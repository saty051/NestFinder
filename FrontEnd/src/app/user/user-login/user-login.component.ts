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
[x: string]: unknown;
  
 @ViewChild('loginForm') 
 loginForm!: NgForm;

 constructor(private authService: AuthService,
  private alertify: AlertifyService,
  private router: Router
) {
// Constructor logic will be added here later
}

  ngOnInit() {
    // Initialization logic will be added here later
  }
  

  onLogin(){
    console.log(this.loginForm.value);
    this.authService.authUser(this.loginForm.value).subscribe(
      (response: UserForLogin) => {
        console.log(response);
        const user = response;
        localStorage.setItem('token', user.token);
        localStorage.setItem('userName', user.userName);
        this.alertify.success("Login Successful");
        this.router.navigate(['/']);
      }, error => {
        console.log(error);
        this.alertify.error(error.error);
      }
    );
    // if(token){
    //   localStorage.setItem('token', token.userName);
    //   this.alertify.success("Login Successful");
    //   this.router.navigate(['/']);
    // } else {
    //   this.alertify.error("User Name or Password is wrong");
    // }
  }
}
