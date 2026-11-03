import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ApiService } from '../core/api.service';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { AuthState } from '../core/auth';
import { Profile } from '../models/api.models';

@Component({ standalone:true, imports:[CommonModule,FormsModule,RouterLink], template:`
<div class="eyebrow">{{signInMode ? (adminMode ? 'ADMINISTRATOR ACCESS' : 'MEMBER ACCESS') : 'MEMBER ACCOUNT'}}</div>
<h1 *ngIf="signInMode">{{adminMode ? 'Admin sign in' : 'User sign in'}} <em>to continue.</em></h1>
<h1 *ngIf="!signInMode">Your account <em>and details.</em></h1>
<div *ngIf="signInMode" class="row g-4 mt-2"><div class="col-lg-6"><section class="panel"><h2>{{adminMode?'Admin sign in':'User sign in'}}</h2><form (ngSubmit)="login()"><label class="form-label">Email</label><input class="form-control mb-3" type="email" required [(ngModel)]="email" name="email"><label class="form-label">Password</label><input class="form-control mb-3" type="password" required [(ngModel)]="password" name="password"><button class="btn btn-dark" [disabled]="busy">{{busy?'Signing in…':'Sign in'}}</button></form><p class="mt-3 mb-0" *ngIf="adminMode">Regular account? <a routerLink="/user-sign-in">User sign in</a></p><p class="mt-3 mb-0" *ngIf="!adminMode">Administrator? <a routerLink="/admin-sign-in">Admin sign in</a> · <a routerLink="/account">Create an account</a></p></section></div></div>
<div *ngIf="!signInMode" class="row g-4 mt-2"><div class="col-lg-5"><section class="panel"><h2>Create user account</h2><form (ngSubmit)="register()"><label class="form-label">Name</label><input class="form-control mb-2" required [(ngModel)]="name" name="name"><label class="form-label">Email</label><input class="form-control mb-2" type="email" required [(ngModel)]="email" name="email"><label class="form-label">Password · at least 8 characters</label><input class="form-control mb-3" type="password" minlength="8" required [(ngModel)]="password" name="password"><button class="btn btn-dark">Register</button></form><p class="mt-3">Already registered? <a routerLink="/user-sign-in">User sign in</a></p><p>Administrator? <a routerLink="/admin-sign-in">Admin sign in</a></p></section></div><div class="col-lg-7"><section class="panel"><button class="btn btn-outline-dark float-end" (click)="getMe()">Refresh profile</button><h2>Profile</h2><p *ngIf="profile">{{profile.email}} · {{profile.role}}</p><p *ngIf="!profile&&!auth.token" class="muted">Sign in to view your profile.</p><form *ngIf="profile" (ngSubmit)="save()"><label class="form-label">Name</label><div class="input-group"><input class="form-control" name="profileName" required [(ngModel)]="name"><button class="btn btn-dark">Update name</button></div></form><button *ngIf="auth.token" class="btn btn-link text-danger px-0 mt-3" (click)="logout()">Sign out</button></section></div></div>
<div class="alert alert-danger mt-3" *ngIf="error" role="alert">{{error}}</div><div class="alert alert-success mt-3" *ngIf="message" role="status">{{message}}</div>`})
export class AccountPage {
 api=inject(ApiService);auth=inject(AuthState);route=inject(ActivatedRoute);router=inject(Router);
 email='';password='';name='';profile:Profile|null=null;error='';message='';busy=false;
 get signInMode(){return this.route.snapshot.data['signIn']===true}
 get adminMode(){return this.route.snapshot.data['admin']===true}
 login(){this.error='';this.message='';this.busy=true;this.api.login({Email:this.email,Password:this.password}).subscribe({next:result=>{this.auth.set(result.accessToken);this.api.me().subscribe({next:profile=>{this.busy=false;this.profile=profile;this.name=profile.name;if(this.adminMode&&profile.role.toLowerCase()!=='admin'){this.auth.clear();this.profile=null;this.error='This account does not have administrator access.';return}this.auth.profile=profile;this.message='Signed in successfully.';const target=this.route.snapshot.queryParamMap.get('returnUrl');this.router.navigateByUrl(target?.startsWith(this.adminMode?'/manage':'/')?target:(profile.role.toLowerCase()==='admin'?'/manage/movies':'/'))},error:e=>{this.busy=false;this.auth.clear();this.fail(e)}})},error:e=>{this.busy=false;this.fail(e)}})}
 register(){this.api.register({Name:this.name,Email:this.email,Password:this.password}).subscribe({next:x=>{this.auth.set(x.accessToken);this.api.me().subscribe({next:p=>{this.profile=p;this.auth.profile=p;this.message='Account created.'},error:e=>this.fail(e)})},error:e=>this.fail(e)})}
 getMe(){this.api.me().subscribe({next:p=>{this.profile=p;this.auth.profile=p;this.name=p.name},error:e=>this.fail(e)})}
 save(){this.api.profile(this.name).subscribe({next:p=>{this.profile=p;this.auth.profile=p;this.message='Profile updated.'},error:e=>this.fail(e)})}
 logout(){this.auth.clear();this.profile=null;this.message='Signed out.';this.router.navigateByUrl('/')}
 fail(e:any){const p=e?.error;this.error=typeof p==='string'?p:p?.title||p?.detail||p?.message||(e?.status===0?'Cannot reach the API. Check that the backend is running.':'Request failed. Please check your details and try again.');this.message=''}
}
