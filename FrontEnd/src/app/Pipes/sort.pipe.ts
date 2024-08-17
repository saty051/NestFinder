import { Pipe, PipeTransform } from '@angular/core';

// Define a custom pipe named 'sort'
@Pipe({
  name: 'sort'  // Name of the pipe, used for referencing it in templates
})
export class SortPipe implements PipeTransform {

  // The transform method is used to sort an array based on the provided field and order
  transform(value: string[], args: any[]): any {
    // Extract sorting field and order from the arguments
    const sortField = args[0]; // The property name to sort by
    const sortOrder = args[1]; // The order of sorting: 'asc' for ascending or 'desc' for descending
    
    // Initialize multiplier for sorting order
    let multiplier = 1; // Default to ascending order

    // If sortOrder is 'desc', set multiplier to -1 for descending order
    if (sortOrder === 'desc') {
      multiplier = -1;
    }

    // Check if the value (array) is defined
    if (value) {
      // Sort the array based on the sortField and sortOrder
      value.sort((a, b) => {
        // Compare the values of the sortField in both items
        if (a[sortField] < b[sortField]) {
          return -1 * multiplier; // Return negative value if in ascending order, positive if in descending
        } else if (a[sortField] > b[sortField]) {
          return 1 * multiplier; // Return positive value if in ascending order, negative if in descending
        } else {
          return 0; // Return 0 if the values are equal
        }
      });

      // Return the sorted array
      return value;
    }
  }
}
