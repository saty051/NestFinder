import { IPropertyBase } from "./Ipropertybase";

export class Property implements IPropertyBase {
    id!: number;
    name!: string;
    propertyType!: string;
    price!: number;
    bhk!: number;
    readyToMove!: number;
    Address!: string;
    Address2?: string;
    city!: string;
    FloorNo?: number;
    TotalFloor?: number;
    PossessionOn!: string;
    image?: string;
    furnishingType!: string;
    sellRent!: number | null;
    builtArea!: number;
    CarpetArea?: number;
    Security?: number;
    Maintenance?: number;
    Gated?: number;
    MainEntrance?: string;
    AOP?: string;
    Description?: string;
    PostedOn!: string;
    PostedBy!: number;
}
