import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, Router, RouterStateSnapshot } from '@angular/router';
import { catchError, EMPTY, Observable, of } from 'rxjs';
import { Property } from 'src/app/model/property';
import { HousingService } from 'src/app/services/housing.service';

@Injectable({
  providedIn: 'root'
})
export class PropertyDetailResolverService implements Resolve<Property> {

constructor(private router: Router,  private housingservice: HousingService) { }
resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<Property> | Property {
  const propId = route.params['id'];
  return this.housingservice.getProperty(+propId).pipe(
    catchError(error => {
      console.error('Error fetching property details', error);
      this.router.navigate(['/']);
      return EMPTY;
    })
  );
}
}
