import { IPropertyBase } from "./Ipropertybase";
import { Photo } from "./photo";

export class Property implements IPropertyBase {
    id!: number;
    sellRent!: number | null;
    name!: string;
    propertyTypeId!: number;
    propertyType!: string | null;
    bhk!: number;
    furnishingTypeId!: number;
    furnishingType!: string | null;
    price!: number;
    builtArea!: number;
    carpetArea?: number;
    address!: string;
    address2?: string;
    CityId!: number;
    city!: string;
    floorNo?: number;
    totalFloors?: number;
    readyToMove!: boolean | null;
    age?: string;
    mainEntrance?: string;
    security?: number;
    gated?: boolean;
    maintenance?: number;
    estPossessionOn!: string;
    photo?: string;
    description?: string;
    latestUpdatedBy?: number;
    photos?: Photo[];
}
