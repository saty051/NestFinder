/* eslint-disable @typescript-eslint/no-unused-vars */
/* eslint-disable @angular-eslint/use-lifecycle-interface */
/* eslint-disable @typescript-eslint/no-explicit-any */
import { Component, EventEmitter, Input, Output, ViewChild, ElementRef } from '@angular/core';
import { HousingService } from 'src/app/services/housing.service';
import { Photo } from 'src/app/model/photo';
import { Property } from 'src/app/model/property';
import { FileUploader } from 'ng2-file-upload';
import { environment } from 'src/environments/environment';
import { AuthService } from 'src/app/services/auth.service';
import { AlertifyService } from 'src/app/services/alertify.service';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent {
  @Input() property!: Property;
  @Output() mainPhotoChangedEvent = new EventEmitter<string>();
  @ViewChild('fileInput') fileInput!: ElementRef; // Reference to the file input

  uploader!: FileUploader;
  baseUrl = environment.baseUrl;
  maxAllowedFileSize = 10 * 1024 * 1024;
  uploadProgress = 0; // Track the progress

  constructor(
    private housingService: HousingService,
    private authService: AuthService,
    private alertify: AlertifyService
  ) {}

  initializeFileUploader() {
    this.uploader = new FileUploader({
      url: `${this.baseUrl}Property/add/photo/${String(this.property.id)}`,
      authToken: `Bearer ${localStorage.getItem('token')}`,
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: true,
      maxFileSize: this.maxAllowedFileSize
    });

    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false;
    };

    this.uploader.onProgressAll = (progress: number) => {
      this.uploadProgress = progress; // Update progress for the progress bar
      console.log(`File upload progress: ${progress}%`);
    };

    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response) {
        const photo: Photo = JSON.parse(response);
        this.property.photos?.push(photo);
        this.uploadProgress = 0; // Reset progress after upload is done
      }
    };
  }

  // Trigger the hidden file input when the icon is clicked
  triggerFileInput() {
    // Log to check authorization
    console.log('Property latestUpdatedBy:', this.property.latestUpdatedBy);
    console.log('Logged-in user ID:', this.authService.getUserId());

    if (this.property.latestUpdatedBy === this.authService.getUserId()) {
      this.fileInput.nativeElement.click();
    } else {
      this.alertify.error('You are not authorized to upload photos.');
    }
  }

  // Handle multiple files selected from the local machine
  onFileSelected(event: any) {
    const files: FileList = event.target.files;
    if (files) {
      for (let i = 0; i < files.length; i++) {
        this.uploader.addToQueue([files[i]]); // Add each file to the uploader queue
      }
    }
  }

  // Handle keydown events for accessibility
  handleKeydown(event: KeyboardEvent) {
    if (event.key === 'Enter') {
      this.triggerFileInput();
    }
  }

  mainPhotoChanged(url: string) {
    this.mainPhotoChangedEvent.emit(url);
  }

  ngOnInit(): void {
    this.initializeFileUploader();
    console.log('Property details:', this.property);
  }

  setPrimaryPhoto(propertyId: number, photo: Photo) {
    this.housingService.setPrimaryPhoto(propertyId, photo.publicId).subscribe({
      next: () => {
        this.mainPhotoChanged(photo.imageUrl);
        this.property.photos?.forEach((p) => {
          if (p.isPrimary) {
            p.isPrimary = false;
          }
          if (p.publicId === photo.publicId) {
            p.isPrimary = true;
          }
        });
        this.alertify.success('Primary photo set successfully.');
      },
      error: (error) => {
        console.error('Error setting primary photo:', error);
        this.alertify.error('Failed to set the primary photo. Please try again.');
      }
    });
  }
  

  deletePhoto(propertyId: number, photo: Photo) {
    this.housingService.deletePhoto(propertyId, photo.publicId).subscribe({
      next: () => {
        this.property.photos = this.property.photos?.filter((p) => p.publicId !== photo.publicId);
      },
      error: (error) => {
        this.alertify.error('An error occurred while deleting the photo');
      }
    });
  }  
}
