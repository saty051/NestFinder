/* eslint-disable @typescript-eslint/no-explicit-any */
import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'filter'  // Name of the pipe, used for referencing it in templates
})
export class FilterPipe implements PipeTransform {

  transform(value: any[], filterString: string, propName: string): any[] {
    if (!value || value.length === 0 || !filterString || !propName) {
      return value;
    }

    const resultArray = [];

    // Ensure case insensitivity by converting both values to lowercase
    for (const item of value) {
      if (item[propName.toLowerCase()] && item[propName.toLowerCase()].toString().toLowerCase().includes(filterString.toLowerCase())) {
        resultArray.push(item);
      }
    }

    return resultArray;
  }
}
