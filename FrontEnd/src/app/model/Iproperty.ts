// Importing the IPropertyBase interface for inheritance.
import { IPropertyBase } from "./Ipropertybase";

// Iproperty interface extends IPropertyBase, adding custom properties.
export interface Iproperty extends IPropertyBase {
    // Description property to hold a textual description of the property.
    Description: string;
}