import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { AlertifyService } from 'src/app/services/alertify.service';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-user-login',
  templateUrl: './user-login.component.html',
  styleUrls: ['./user-login.component.css']
})
export class UserLoginComponent implements OnInit{
[x: string]: any;
  
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
    const token = this.authService.authUser(this.loginForm.value);
    if(token){
      localStorage.setItem('token', token.userName);
      this.alertify.success("Login Successful");
      this.router.navigate(['/']);
    } else {
      this.alertify.error("User Name or Password is wrong");
    }
  }
}
