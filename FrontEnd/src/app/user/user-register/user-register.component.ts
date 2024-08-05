import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { User } from 'src/app/model/user';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-user-register',
  templateUrl: './user-register.component.html',
  styleUrls: ['./user-register.component.css']
})
export class UserRegisterComponent {
  registrationForm!: FormGroup;
  user!: User;
  userSubmitted!: boolean;
  constructor(private fb: FormBuilder, private userService: UserService){ }

  ngOnInit(){
    this.registrationForm = this.fb.group({
      username: ['',[Validators.required, Validators.minLength(3)]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(8)]],
      confirmpassword: ['', [Validators.required]],
      mobile: ['', [Validators.required, Validators.maxLength(10)]]
    }, {validators: this.passwordMismatchValidator})
  }

  passwordMismatchValidator(fg: FormGroup): Validators| null{
    return fg.get('password')?.value === fg.get('confirmpassword')?.value ? null : {mismatch: true}
  }

  onSubmit(){
    console.log(this.registrationForm.value);
    this.userSubmitted = true;

    if(this.registrationForm.valid){
     // this.user = Object.assign(this.user,this.registrationForm.value);
      this.userService.addUser(this.UserData());
      this.registrationForm.reset();
      this.userSubmitted = false;
    } 
  }

  UserData(): User{
    return this.user = {
      userName: this.username.value,
      email: this.email.value,
      password: this.password.value,
      mobile: this.mobile.value
    };
  }

// ------------------------------------
// Getter methods for all form Control
// ------------------------------------

  get username(){
    return this.registrationForm.get('username') as FormControl;
  }
  get email(){
    return this.registrationForm.get('email') as FormControl;
  }
  get password(){
    return this.registrationForm.get('password') as FormControl;
  }
  get confirmpassword(){
    return this.registrationForm.get('confirmpassword') as FormControl;
  }
  get mobile(){
    return this.registrationForm.get('mobile') as FormControl;
  }


}
