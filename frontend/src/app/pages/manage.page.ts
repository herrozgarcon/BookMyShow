import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ApiService } from '../core/api.service';
import { finalize } from 'rxjs';

type Resource = 'movies'|'theatres'|'screens'|'seats'|'shows'|'bookings';
type Action = 'create'|'update'|'delete'|'reload';
interface ResourceConfig { label:string; list:string; byId?:(id:number)=>string; create?:string; update?:(id:number)=>string; remove?:(id:number)=>string; idLabel?:string; hint:string; initialBody:string; }

@Component({standalone:true,imports:[CommonModule,FormsModule],template:`
<div class="eyebrow">OPERATIONS</div><h1>Management <em>workspace.</em></h1>
<div class="row g-3 align-items-end mt-2">
 <div class="col-md-3"><label class="form-label" for="resource">Resource</label><select id="resource" class="form-select" [(ngModel)]="kind" (ngModelChange)="resourceChanged()"><option *ngFor="let k of kinds" [value]="k">{{configs[k].label}}</option></select></div>
 <div class="col-md-3"><label class="form-label" for="resource-id">{{config.idLabel}}</label><input id="resource-id" class="form-control" type="number" min="1" [(ngModel)]="id"></div>
 <div class="col-md-2" *ngIf="kind==='shows'"><label class="form-label" for="show-filter">Filter</label><select id="show-filter" class="form-select" [(ngModel)]="showFilter"><option value="movie">Movie</option><option value="theatre">Theatre</option></select></div>
 <div class="col-md-2"><button class="btn btn-outline-dark w-100" (click)="load(true)" [disabled]="busy">Search / reload</button></div>
</div>
<p class="muted mt-2">{{config.hint}}</p>
<div class="d-flex flex-wrap gap-2 mb-3"><button *ngIf="config.create" class="btn btn-dark" (click)="save('create')" [disabled]="busy">{{busy&&activeAction==='create'?'Creating…':'Create'}}</button><button *ngIf="config.update" class="btn btn-outline-dark" (click)="save('update')" [disabled]="busy||!validId">{{busy&&activeAction==='update'?'Updating…':'Update selected ID'}}</button><button *ngIf="config.remove" class="btn btn-outline-danger" (click)="remove()" [disabled]="busy||!validId">{{busy&&activeAction==='delete'?'Deleting…':'Delete selected ID'}}</button><button *ngIf="kind==='seats'" class="btn btn-outline-dark" (click)="save('bulk')" [disabled]="busy">Bulk create seats</button></div>
<label class="form-label" for="body" *ngIf="config.create||config.update">Request body · match the resource DTO</label><textarea *ngIf="config.create||config.update" id="body" class="form-control code" rows="8" [(ngModel)]="body"></textarea>
<div class="alert alert-info mt-3" *ngIf="busy" role="status">{{busyMessage}}</div><div class="alert alert-danger mt-3" *ngIf="error" role="alert">{{error}}</div><div class="alert alert-success mt-3" *ngIf="message" role="status">{{message}}</div>
<section *ngIf="items.length" class="mt-4"><h2>{{config.label}} results</h2><article class="panel mb-2" *ngFor="let item of items"><pre class="result mb-0">{{item|json}}</pre></article></section><div class="empty" *ngIf="loaded&&!busy&&!items.length&&!error">No {{config.label.toLowerCase()}} found.</div>
`,})
export class ManagePage {
 api=inject(ApiService); route=inject(ActivatedRoute);
 kinds:Resource[]=['movies','theatres','screens','seats','shows','bookings'];
 configs:Record<Resource,ResourceConfig>={
  movies:{label:'Movies',list:'/api/movies',byId:id=>`/api/movies/${id}`,create:'/api/movies',update:id=>`/api/movies/${id}`,remove:id=>`/api/movies/${id}`,idLabel:'Movie ID',hint:'Movie DTO: Title, Description, Genre, ReleaseDate, DurationMinutes (or duration), language. Duration must be a positive number.',initialBody:'{\n  "Title": "",\n  "Description": "",\n  "Genre": "",\n  "ReleaseDate": "YYYY-MM-DD",\n  "DurationMinutes": 0,\n  "language": ""\n}'},
  theatres:{label:'Theatres',list:'/api/theatre',byId:id=>`/api/theatre/${id}`,create:'/api/theatre',update:id=>`/api/theatre/${id}`,remove:id=>`/api/theatre/${id}`,idLabel:'Theatre ID',hint:'Theatre DTO: Name and Location.',initialBody:'{\n  "Name": "",\n  "Location": ""\n}'},
  screens:{label:'Screens',list:'/api/screens',byId:id=>`/api/screens/search?id=${id}`,create:'/api/screens',update:id=>`/api/screens/update?id=${id}`,remove:id=>`/api/screens/delete?id=${id}`,idLabel:'Screen ID',hint:'Screen DTO: ScreenName and TheatreId.',initialBody:'{\n  "ScreenName": "",\n  "TheatreId": 0\n}'},
  seats:{label:'Seats',list:'/api/Seats/screen/',byId:id=>`/api/Seats/${id}`,create:'/api/Seats',update:id=>`/api/Seats/${id}`,remove:id=>`/api/Seats/${id}`,idLabel:'Screen ID for list; Seat ID for update/delete',hint:'Seat DTO: ScreensId, RowName, SeatNumber, SeatType. Bulk body: ScreensId and Seats array.',initialBody:'{\n  "ScreensId": 0,\n  "RowName": "A",\n  "SeatNumber": 1,\n  "SeatType": "Standard"\n}'},
  shows:{label:'Shows',list:'/api/Shows',byId:id=>`/api/Shows/${id}`,create:'/api/Shows',update:id=>`/api/Shows/${id}`,idLabel:'Show ID or filter ID',hint:'Show DTO: MovieId, ScreensId, StartTime, EndTime, TicketPrice.',initialBody:'{\n  "MovieId": 0,\n  "ScreensId": 0,\n  "StartTime": "YYYY-MM-DDTHH:mm:ss",\n  "EndTime": "YYYY-MM-DDTHH:mm:ss",\n  "TicketPrice": 0\n}'},
  bookings:{label:'Bookings',list:'/api/Bookings',byId:id=>`/api/Bookings/id?id=${id}`,idLabel:'Booking ID',hint:'Bookings are customer-owned. This workspace supports lookup only; the backend exposes no admin create/update/delete operation.',initialBody:'{}'}
 };
 kind:Resource='movies';id=0;body='';items:any[]=[];busy=false;loaded=false;activeAction:Action|null=null;error='';message='';showFilter:'movie'|'theatre'='movie';
 get config(){return this.configs[this.kind]} get validId(){return Number.isInteger(+this.id)&&+this.id>0} get busyMessage(){return this.activeAction==='reload'?'Loading…':'Saving changes…'}
 ngOnInit(){const candidate=this.route.snapshot.paramMap.get('kind') as Resource|null;this.kind=candidate&&this.kinds.includes(candidate)?candidate:'movies';this.body=this.config.initialBody;this.load()}
 resourceChanged(){this.id=0;this.items=[];this.error='';this.message='';this.loaded=false;this.body=this.config.initialBody;this.load()}
 load(search=false){if(this.busy)return;this.begin('reload');let url=this.config.list;if(search&&this.validId){if(this.kind==='seats')url=this.config.byId?.(+this.id)||url;if(this.kind==='movies'||this.kind==='theatres'||this.kind==='screens'||this.kind==='bookings')url=this.config.byId?.(+this.id)||url;if(this.kind==='shows')url=`/api/Shows/${this.showFilter}/${this.id}`;}else if(this.kind==='seats'&&this.validId)url+=this.id;
 this.api.get<any>(url).pipe(finalize(()=>this.end())).subscribe({next:r=>{this.items=Array.isArray(r)?r:r==null?[]:[r];this.loaded=true;this.error='';this.message=''},error:e=>{this.items=[];this.loaded=true;this.fail(e)}})}
 save(action:'create'|'update'|'bulk'){if(this.busy)return;const actualAction=action==='bulk'?'create':action;if(action==='update'&&!this.validId){this.error='Enter a valid resource ID before updating.';this.message='';return}let payload:any;try{payload=JSON.parse(this.body)}catch{this.error='Request body is not valid JSON.';this.message='';return}const url=action==='bulk'?'/api/Seats/bulk':action==='update'?this.config.update?.(+this.id):this.config.create;if(!url){this.error=`${this.config.label} does not support ${actualAction}.`;return}this.begin(actualAction);const req=action==='update'?this.api.put<any>(url,payload):this.api.post<any>(url,payload);req.pipe(finalize(()=>this.end())).subscribe({next:()=>{this.error='';this.message=`${this.config.label.slice(0,-1)||this.config.label} ${action==='update'?'updated':'created'} successfully.`;this.loadAfterMutation(payload)},error:e=>this.fail(e)})}
 remove(){if(this.busy)return;if(!this.config.remove){this.error=`${this.config.label} does not support delete.`;return}if(!this.validId){this.error='Enter a valid resource ID before deleting.';return}if(!confirm(`Delete ${this.config.label.toLowerCase().replace(/s$/,'')} ${this.id}?`))return;this.begin('delete');this.api.del<any>(this.config.remove(+this.id)).pipe(finalize(()=>this.end())).subscribe({next:()=>{this.error='';this.message=`${this.config.label.slice(0,-1)||this.config.label} deleted successfully.`;this.id=0;this.loadAfterMutation()},error:e=>this.fail(e)})}
 private loadAfterMutation(payload:any){let url=this.config.list;if(this.kind==='seats'){const screenId=Number(payload?.ScreensId??payload?.screensId)||Number(this.id);if(screenId>0)url+=screenId;}this.api.get<any>(url).subscribe({next:r=>{this.items=Array.isArray(r)?r:r==null?[]:[r];this.loaded=true},error:e=>{this.items=[];this.loaded=true;this.fail(e)}})}
 private begin(action:Action){this.busy=true;this.activeAction=action;this.error='';if(action==='reload')this.message=''} private end(){this.busy=false;this.activeAction=null}
 private fail(e:any){const p=e?.error;let detail=typeof p==='string'?p:p?.detail||p?.title||p?.message;if(p?.errors&&typeof p.errors==='object')detail=Object.values(p.errors).flat().join(' ');this.error=detail|| (e?.status===0?'Could not reach the API. Check that the backend is running.':`Request failed with HTTP ${e?.status||'error'}.`);this.message=''}
}
