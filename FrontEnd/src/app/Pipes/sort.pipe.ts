import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'sort'
})
export class SortPipe implements PipeTransform {

  transform(value: string[], args: any[]): any {
    const sortField = args[0];
    const sortOrder = args[1];
    let multiplier = 1;

    if (sortOrder === 'desc') {
      multiplier = -1;
    }
    const result = value.sort((a, b) => {
      if (a[sortField] < b[sortField]) {
        return -1 * multiplier;
      } else if (a[sortField] > b[sortField]) {
        return 1 * multiplier;
      } else {
        return 0;
      }
    });
    return result;
  }

}
