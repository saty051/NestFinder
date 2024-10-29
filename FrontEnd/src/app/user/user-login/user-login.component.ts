/* eslint-disable @typescript-eslint/no-unused-vars */
/* eslint-disable @angular-eslint/no-empty-lifecycle-method */
/* eslint-disable @typescript-eslint/no-empty-function */
import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, NgForm, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AlertifyService } from 'src/app/services/alertify.service';
import { AuthService } from 'src/app/services/auth.service';
import { SecurityQuestion } from 'src/app/Enums/security-question.enum';
import { UserForLogin } from 'src/app/model/user';

@Component({
  selector: 'app-user-login',
  templateUrl: './user-login.component.html',
  styleUrls: ['./user-login.component.css']
})
export class UserLoginComponent implements OnInit {
  @ViewChild('loginForm') loginForm!: NgForm;
  showForgotPasswordButton = false;
  isForgotPasswordActive = false;
  forgotPasswordForm!: FormGroup;
  usernameOnFailedLogin = '';

  securityQuestions: { value: SecurityQuestion; label: string }[] = [
    { value: SecurityQuestion.WhatIsYourFavoriteColor, label: 'What is your favorite color?' },
    { value: SecurityQuestion.WhatCityWereYouBornIn, label: 'What city were you born in?' },
    { value: SecurityQuestion.WhatIsYourMotherMaidenName, label: 'What is your mother’s maiden name?' },
    { value: SecurityQuestion.WhatIsTheNameOfYourFirstPet, label: 'What is the name of your first pet?' },
    { value: SecurityQuestion.WhatIsYourFavoriteBook, label: 'What is your favorite book?' },
    { value: SecurityQuestion.WhatWasTheNameOfYourElementarySchool, label: 'What was the name of your elementary school?' },
    { value: SecurityQuestion.WhatIsYourFavoriteChildhoodMovie, label: 'What is your favorite childhood movie?' },
    { value: SecurityQuestion.WhatIsTheNameOfYourBestChildhoodFriend, label: 'What is the name of your best childhood friend?' },
    { value: SecurityQuestion.WhatWasTheMakeOfYourFirstCar, label: 'What was the make of your first car?' },
    { value: SecurityQuestion.WhatIsTheNameOfTheStreetYouGrewUpOn, label: 'What is the name of the street you grew up on?' }
  ];

  selectedQuestion = '';  // Add this line
    securityAnswer = '';
    newPassword = '';


  constructor(
    private authService: AuthService,
    private alertify: AlertifyService,
    private router: Router,
    private fb: FormBuilder
  ) {
    this.createForgotPasswordForm();
  }

  ngOnInit() {}

  createForgotPasswordForm() {
    this.forgotPasswordForm = this.fb.group({
      username: ['', Validators.required],
      securityQuestion: ['', Validators.required],
      securityAnswer: ['', Validators.required],
      newPassword: ['', [Validators.required, Validators.minLength(8)]]
    });
  }

  onLogin() {
    if (this.loginForm.valid) {
      this.authService.authUser(this.loginForm.value).subscribe({
        next: (response: UserForLogin) => {
          const user = response;
          localStorage.setItem('token', user.token);
          localStorage.setItem('userName', user.userName);
          this.alertify.success("Login Successful");
          this.showForgotPasswordButton = false;
          this.isForgotPasswordActive = false;
          this.router.navigate(['/']);
        },
        error: (error) => {
          this.alertify.error('Invalid Username or Password');
          this.showForgotPasswordButton = true;
          this.usernameOnFailedLogin = this.loginForm.value.userName;
        }
      });
    }
  }
  

  onForgotPassword() {
    this.isForgotPasswordActive = true;
    this.showForgotPasswordButton = false;
    this.forgotPasswordForm.patchValue({ username: this.loginForm.value.userName });
  }

  onResetPassword() {
    if (this.forgotPasswordForm.valid) {
        const username = this.usernameOnFailedLogin;
        const securityQuestionValue = this.forgotPasswordForm.get('securityQuestion')?.value;
        const securityAnswer = this.forgotPasswordForm.get('securityAnswer')?.value;
        const newPassword = this.forgotPasswordForm.get('newPassword')?.value;

        if (!username || securityQuestionValue === null || !securityAnswer || !newPassword) {
            this.alertify.error('Please fill out all fields.');
            return;
        }

        // Convert the security question value to its corresponding string
        const securityQuestionString = SecurityQuestion[securityQuestionValue as keyof typeof SecurityQuestion];

        const resetData = {
            username: username,
            securityQuestion: securityQuestionString, // Use the string representation of the enum
            securityAnswer: securityAnswer,
            newPassword: newPassword
        };

        console.log("Reset Data Payload:", resetData); // Debugging payload structure

        this.authService.resetPassword(resetData).subscribe({
            next: () => {
                this.alertify.success('Password reset successful. Please log in with your new password.');
                this.isForgotPasswordActive = false;
            },
            error: (error) => {
                this.alertify.error(error.error.errorMessage || 'Password reset failed. Please try again.');
            }
        });
    }
}

  onCancel() {
    this.isForgotPasswordActive = false;
    this.showForgotPasswordButton = false;
    this.router.navigate(['/login']);
  }
}
