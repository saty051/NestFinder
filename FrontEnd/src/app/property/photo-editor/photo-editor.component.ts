/* eslint-disable @typescript-eslint/no-empty-function */
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { HousingService } from 'src/app/services/housing.service';
import { Photo } from 'src/app/model/photo';
import { Property } from 'src/app/model/property';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent {
@Input() property!: Property;
@Output() mainPhotoChangedEvent = new EventEmitter<string>();

constructor(private housingService: HousingService) { }

mainPhotoChanged(url: string) {
  this.mainPhotoChangedEvent.emit(url);
}
setPrimaryPhoto(propertyId: number, photo: Photo) {
  this.housingService.setPrimaryPhoto(propertyId, photo.publicId).subscribe(() => {
      this.mainPhotoChanged(photo.imageUrl);
      this.property.photos?.forEach(p => {
        if (p.isPrimary) {p.isPrimary = false;}
        if(p.publicId === photo.publicId) {p.isPrimary = true;}
      });
    });
  }

  deletePhoto(propertyId: number, photo: Photo) {
    this.housingService.deletePhoto(propertyId, photo.publicId).subscribe(() => {
        this.property.photos = this.property.photos?.filter(p => p.publicId !== photo.publicId);
      });
    }
}
