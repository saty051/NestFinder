/* eslint-disable @angular-eslint/no-empty-lifecycle-method */
import { Component, OnInit } from '@angular/core';
import { AlertifyService } from '../services/alertify.service';

// Component decorator to define metadata for the NavBarComponent
@Component({
  selector: 'app-nav-bar',                // Selector to use this component in templates
  templateUrl: './nav-bar.component.html', // Path to the component's HTML template
  styleUrls: ['./nav-bar.component.css']   // Path to the component's CSS styles
})
export class NavBarComponent implements OnInit {
  // Property to store the logged-in user's information
  loggedInUser!: string | null;

  // Injecting AlertifyService to handle notifications
  constructor(private alertify: AlertifyService) { }

  // Lifecycle hook to perform initialization logic
  ngOnInit() {
      // You can add initialization logic here if needed
  }

  // Method to check if the user is logged in and retrieve their information
  loggedIn() {
    // Retrieve the token from local storage, which indicates if the user is logged in
    this.loggedInUser = localStorage.getItem('userName');
    // Return the token (or null) based on its presence
    return this.loggedInUser;
  }

  // Method to handle user logout
  onLogout() {
    // Remove the token from local storage to log the user out
    localStorage.removeItem('token');
    localStorage.removeItem('userName');
    // Display a success message using AlertifyService
    this.alertify.success("You are logged out!");
  }
}
