/* eslint-disable @typescript-eslint/no-unused-vars */
/* eslint-disable @typescript-eslint/no-empty-function */
/* eslint-disable @angular-eslint/no-empty-lifecycle-method */
import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { UserForLogin } from 'src/app/model/user';
import { AlertifyService } from 'src/app/services/alertify.service';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-user-login',
  templateUrl: './user-login.component.html',
  styleUrls: ['./user-login.component.css']
})
export class UserLoginComponent implements OnInit{
 @ViewChild('loginForm') 
 loginForm!: NgForm;

 constructor(private authService: AuthService,
  private alertify: AlertifyService,
  private router: Router
) {}

  ngOnInit() {}

  onLogin() {
    console.log(this.loginForm.value);
    this.authService.authUser(this.loginForm.value).subscribe(
      (response: UserForLogin) => {
        console.log(response);
        const user = response;
        localStorage.setItem('token', user.token);
        localStorage.setItem('userName', user.userName);
        this.alertify.success("Login Successful");
        this.router.navigate(['/']);
      },
      (error) => {
        this.alertify.error('You are not authorized to perform this action.');
      }
    );
  }
}
