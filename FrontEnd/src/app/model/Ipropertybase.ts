// Defining the IPropertyBase interface which serves as a base structure for property-related data.
export interface IPropertyBase {
    // Unique identifier for the property; can be null if not assigned.
    Id: number | null;

    // Indicates whether the property is for sale (1) or rent (2); can be null.
    SellRent: number | null;

    // Name of the property; can be null if not provided.
    Name: string | null;

    // Type of property (e.g., apartment, villa); can be null.
    PType: string | null;

    // Furnishing type (e.g., furnished, semi-furnished, unfurnished); can be null.
    FType: string | null;

    // Number of bedrooms in the property; can be null if not specified.
    BHK: number | null;

    // City where the property is located; can be null.
    City: string | null;

    // Ready To Move value indicator; can be null if not applicable.
    RTM: number | null;

    // Built-up area of the property in square feet; can be null.
    BuiltArea: number | null;

    // Price of the property; can be null if not assigned.
    Price: number | null;

    // Optional property for storing an image URL; can be null if no image is provided.
    Image?: string | null;

    // Optional property indicating estimated possession date; can be undefined if not specified.
    estPossessionOn?: string;
}
