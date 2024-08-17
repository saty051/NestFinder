import { IPropertyBase } from "./Ipropertybase";

// Implementing the IPropertyBase interface to define the structure of a property object.
export class Property implements IPropertyBase {
    // Unique identifier for the property; initialized with a non-null assertion.
    Id!: number;

    // Name of the property; initialized with a non-null assertion.
    Name!: string;

    // Type of property (e.g., apartment, villa); initialized with a non-null assertion.
    PType!: string;

    // Price of the property; initialized with a non-null assertion.
    Price!: number;

    // Number of bedrooms in the property; initialized with a non-null assertion.
    BHK!: number;

    // Real-Time Market value indicator; initialized with a non-null assertion.
    RTM!: number;

    // Address of the property; not part of IPropertyBase, but added for additional details.
    Address!: string;

    // Optional second address line; can be undefined if not provided.
    Address2?: string;

    // City where the property is located; initialized with a non-null assertion.
    City!: string;

    // Optional floor number where the property is located; can be undefined if not specified.
    FloorNo?: number;

    // Optional total number of floors in the building; can be undefined if not specified.
    TotalFloor?: number;

    // Date when the property is available for possession; initialized with a non-null assertion.
    PossessionOn!: string;

    // Optional image URL for the property; can be undefined if no image is provided.
    Image?: string;

    // Furnishing type (e.g., furnished, semi-furnished, unfurnished); initialized with a non-null assertion.
    FType!: string;

    // Indicates whether the property is for sale (1) or rent (2); can be null.
    SellRent!: number | null;

    // Built-up area of the property in square feet; initialized with a non-null assertion.
    BuiltArea!: number;

    // Optional carpet area of the property in square feet; can be undefined if not provided.
    CarpetArea?: number;

    // Optional security deposit amount; can be undefined if not provided.
    Security?: number;

    // Optional maintenance cost; can be undefined if not provided.
    Maintenance?: number;

    // Optional gated community indicator; can be undefined if not applicable.
    Gated?: number;

    // Optional main entrance description; can be undefined if not specified.
    MainEntrance?: string;

    // Optional type of age of property (AOP); can be undefined if not provided.
    AOP?: string;

    // Optional description of the property; can be undefined if not provided.
    Description?: string;

    // Date when the property was posted; initialized with a non-null assertion.
    PostedOn!: string;

    // Identifier for the user who posted the property; initialized with a non-null assertion.
    PostedBy!: number;
}
