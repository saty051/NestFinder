import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Property } from 'src/app/model/property';
import { HousingService } from 'src/app/services/housing.service';
import { LikeService } from 'src/app/services/like.service';
import { AlertifyService } from 'src/app/services/alertify.service';
import { NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { NgxGalleryImage } from '@kolkov/ngx-gallery';
import { NgxGalleryAnimation } from '@kolkov/ngx-gallery';

@Component({
  selector: 'app-property-detail',
  templateUrl: './property-detail.component.html',
  styleUrls: ['./property-detail.component.css']
})
export class PropertyDetailComponent implements OnInit {
  public propertyId!: number;
  public mainPhotoUrl: string | null = null;
  property: Property = new Property();
  galleryOptions!: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[] = [];
  isLiked = false;
  totalLikes = 0;
  errorMessage: string | null = null;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private housingService: HousingService,
    private likeService: LikeService,
    private alertify: AlertifyService
  ) {}

  ngOnInit() {
    this.propertyId = +this.route.snapshot.params['id'];
    this.route.data.subscribe((data) => {
      this.property = data['prp'];
      this.getLikesForProperty();
    });

    if (this.property?.estPossessionOn) {
      this.property.age = this.housingService.getPropertyAge(this.property.estPossessionOn);
    }

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

    this.galleryImages = this.getPropertyPhotos();

    // Only check if the property is liked if the user is logged in
    this.checkIfUserIsLoggedIn();
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

  checkIfUserIsLoggedIn(): void {
    const token = localStorage.getItem('token');
    if (token) {
      this.checkIfLiked(this.propertyId);
    }
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
    const token = localStorage.getItem('token');
    if (!token) {
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