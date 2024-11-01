import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Property } from 'src/app/model/property';
import { HousingService } from 'src/app/services/housing.service';
import { LikeService } from 'src/app/services/like.service';
import { AlertifyService } from 'src/app/services/alertify.service';
import { AuthService } from 'src/app/services/auth.service';
import { NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { NgxGalleryImage } from '@kolkov/ngx-gallery';
import { NgxGalleryAnimation } from '@kolkov/ngx-gallery';

@Component({
  selector: 'app-property-detail',
  templateUrl: './property-detail.component.html',
  styleUrls: ['./property-detail.component.css']
})
export class PropertyDetailComponent implements OnInit {
  public propertyId!: number;  // Stores the ID of the current property
  public mainPhotoUrl: string | null = null;  // Stores the main photo URL if available
  property: Property = new Property();
  contactEmail: string | null = null;
  contactPhone: string | null = null;
  galleryOptions!: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[] = [];
  isLiked = false;
  totalLikes = 0;
  errorMessage: string | null = null;
  isUserLoggedIn = false;

  constructor(
    private route: ActivatedRoute,
    public router: Router,
    private housingService: HousingService,
    private likeService: LikeService,
    private alertify: AlertifyService,
    private authService: AuthService
  ) {}

  ngOnInit() {
    this.propertyId = +this.route.snapshot.params['id'];
    this.route.data.subscribe((data) => {
      this.property = data['prp'];
      this.getLikesForProperty();
    });

    // Check if the user is logged in
    this.isUserLoggedIn = this.authService.loggedIn();

    // If the user is logged in, load contact info
    if (this.isUserLoggedIn) {
      this.fetchContactDetails();
    }

    // Set the property age if possession date is available
    if (this.property?.estPossessionOn) {
      this.property.age = this.housingService.getPropertyAge(this.property.estPossessionOn);
    }

    // Initialize gallery options for property photos
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

    // Load the gallery images for the property
    this.galleryImages = this.getPropertyPhotos();

    // Check if the property is liked, only if the user is logged in
    if (this.isUserLoggedIn) {
      this.checkIfLiked(this.propertyId);
    }
  }

  fetchContactDetails() {
    this.housingService.getContactDetails(this.propertyId).subscribe({
      next: (contact) => {
        this.contactEmail = contact.email;
        this.contactPhone = contact.phoneNumber;
      },
      error: (error) => {
        console.error('Error fetching contact details:', error);
        this.alertify.error('Unable to fetch contact info');
      }
    });
  }

  changePrimaryPhoto(mainPhotoUrl: string) {
    this.mainPhotoUrl = mainPhotoUrl;
  }

  getPropertyPhotos(): NgxGalleryImage[] {
    const photoUrls: NgxGalleryImage[] = [];
    if (this.property?.photos) {
      for (const photo of this.property.photos) {
        if (photo.isPrimary) {
          this.mainPhotoUrl = photo.imageUrl;
        } else {
          photoUrls.push({
            small: photo?.imageUrl,
            medium: photo?.imageUrl,
            big: photo?.imageUrl
          });
        }
      }
    }
    return photoUrls;
  }

  getLikesForProperty(): void {
    this.likeService.getLikesForProperty(this.propertyId).subscribe(
      (likesCount: number) => {
        this.totalLikes = likesCount;
      },
      error => {
        console.error('Error fetching likes for property:', error);
      }
    );
  }

  checkIfLiked(propertyId: number): void {
    this.likeService.isPropertyLiked(propertyId).subscribe(
      (isLiked: boolean) => {
        this.isLiked = isLiked;
      },
      error => {
        console.error('Error checking if property is liked:', error);
      }
    );
  }

  toggleLike(): void {
    if (!this.isUserLoggedIn) {
      this.alertify.error('Kindly log in to like the property');
      this.router.navigate(['/user/login']);
      return;
    }

    if (this.isLiked) {
      this.likeService.removeLike(this.propertyId).subscribe({
        next: () => {
          this.isLiked = false;
          this.totalLikes--;
          this.errorMessage = null; // Clear error message on success
        },
        error: (error) => {
          console.error('Error removing like:', error);
          this.errorMessage = error.message;
        }
      });
    } else {
      this.likeService.addLike(this.propertyId).subscribe({
        next: () => {
          this.isLiked = true;
          this.totalLikes++;
          this.errorMessage = null; // Clear error message on success
        },
        error: (error) => {
          console.error('Error liking property:', error);
          this.errorMessage = error.message;
        }
      });
    }
  }
}
