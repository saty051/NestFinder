import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Property } from 'src/app/model/property';
import { HousingService } from 'src/app/services/housing.service';
import { NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { NgxGalleryImage } from '@kolkov/ngx-gallery';
import { NgxGalleryAnimation } from '@kolkov/ngx-gallery';

@Component({
  selector: 'app-property-detail',
  templateUrl: './property-detail.component.html',
  styleUrls: ['./property-detail.component.css']
})
export class PropertyDetailComponent implements OnInit {

  public propertyId!: number; // Property ID to be fetched from route
  public mainPhotoUrl: string | null = null;
  property: Property = new Property(); // Initialize property as a new instance of Property
  galleryOptions!: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[] = []; // Initialize as an empty array to avoid undefined

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private housingService: HousingService
  ) { }

  ngOnInit() {
    // Get the property ID from the route
    this.propertyId = +this.route.snapshot.params['id']; 

    // Subscribe to the route data using the interface
    this.route.data.subscribe(
      (data) => {
        this.property = data['prp']; // Access the 'prp' property
        if (this.property.photos) {
          console.log(this.property.photos);
        }
      }
    );

    // Calculate the property age
    if (this.property?.estPossessionOn) {
      this.property.age = this.housingService.getPropertyAge(this.property.estPossessionOn);
    }

    // Set gallery options
    this.galleryOptions = [
      {
        width: '100%',
        height: '465px',
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide,
        preview: true,
        previewCloseOnClick: true
      }
    ];

    // Load gallery images
    this.galleryImages = this.getPropertyPhotos();
  }

  getPropertyPhotos(): NgxGalleryImage[] {
    const photoUrls: NgxGalleryImage[] = [];

    if (this.property?.photos) {
      for (const photo of this.property.photos) {
        if (photo.isPrimary) {
          this.mainPhotoUrl = photo.imageUrl;
        }
        else{
        photoUrls.push({
          small: photo?.imageUrl,
          medium: photo?.imageUrl,
          big: photo?.imageUrl
        }
      );}
      }
    }

    return photoUrls;  // Always return the array, even if empty
  }
}
