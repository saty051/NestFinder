import { Pipe, PipeTransform } from '@angular/core';

// Define a custom pipe named 'filter'
@Pipe({
  name: 'filter'  // Name of the pipe, used for referencing it in templates
})
export class FilterPipe implements PipeTransform {

  // The transform method is used to filter an array based on the filterString and propName
  transform(value: any[], filterString: string, propName: string): any[] {
    // Return the original array if it's empty, or if filterString or propName are empty
    if (!value || value.length === 0 || !filterString || !propName) {
      return value;
    }

    // Initialize an empty array to hold the filtered results
    const resultArray = [];

    // Loop through each item in the array
    for (const item of value) {
      // Check if the item has the property and if it contains the filterString (case-insensitive)
      if (item[propName] && item[propName].toString().toLowerCase().includes(filterString.toLowerCase())) {
        // Add the item to the resultArray if it matches the filter
        resultArray.push(item);
      }
    }

    // Return the filtered array
    return resultArray;
  }
}
