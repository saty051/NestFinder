import { IPropertyBase } from "./Ipropertybase";

export class Property implements IPropertyBase {
    id!: number;
    name!: string;
    propertyType!: string;
    price!: number;
    bhk!: number;
    readyToMove!: number;
    address!: string;
    address2?: string;
    city!: string;
    floorNo?: number;
    totalFloors?: number;
    estPossessionOn!: Date;
    image?: string;
    furnishingType!: string;
    sellRent!: number | null;
    builtArea!: number;
    carpetArea?: number;
    security?: number;
    maintenance?: number;
    gated?: boolean;
    mainEntrance?: string;
    age?: string;
    description?: string;
}
