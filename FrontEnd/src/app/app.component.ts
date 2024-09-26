import { Component, OnInit } from '@angular/core';
import { AuthService } from './services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title ='FrontEnd';
  constructor(private authService: AuthService) {}

  ngOnInit(): void {
    window.addEventListener('beforeunload', () => {
      this.authService.clearUserData();
    });

    // Optionally, we can check if the user is logged in and take action
    if (!this.authService.getToken()) {
      this.authService.clearUserData();
    }
  }
}
