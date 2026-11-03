import { Injectable, inject } from '@angular/core';
import { HttpInterceptorFn } from '@angular/common/http';
import { catchError, map, Observable, of, tap, throwError } from 'rxjs';
import { ApiService } from './api.service';
import { Profile } from '../models/api.models';
@Injectable({providedIn:'root'}) export class AuthState {
  private api=inject(ApiService); token=localStorage.getItem('bms-token'); profile:Profile|null=null; checking=false;
  get isAdmin(){return this.profile?.role?.toLowerCase()==='admin'}
  set(token:string){this.token=token;this.profile=null;localStorage.setItem('bms-token',token)}
  clear(){this.token=null;this.profile=null;localStorage.removeItem('bms-token')}
  validate():Observable<Profile|null>{if(!this.token)return of(null);this.checking=true;return this.api.me().pipe(tap(p=>{this.profile=p}),map(p=>p),catchError(e=>{this.clear();return throwError(()=>e)}),tap({finalize:()=>this.checking=false}))}
}
export const authInterceptor:HttpInterceptorFn=(req,next)=>{const state=inject(AuthState);const protectedRequest=req.url.includes('/api/')&&!req.url.includes('/api/Auth/login')&&!req.url.includes('/api/Auth/register');const outgoing=state.token&&protectedRequest?req.clone({setHeaders:{Authorization:`Bearer ${state.token}`}}):req;return next(outgoing).pipe(catchError(e=>{if(e.status===401&&protectedRequest)state.clear();return throwError(()=>e)}))};
