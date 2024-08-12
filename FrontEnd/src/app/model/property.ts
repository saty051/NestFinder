import { IPropertyBase } from "./Ipropertybase";

export class Property implements IPropertyBase{
    Id!: number;
    Name!: string;
    PType!: string;
    Price!: number;
    BHK!: number;
    RTM!: number;
    Address!: string;
    Address2?: string;
    City!: string;
    FloorNo?: number;
    TotalFloor?: number;
    PossessionOn!: string;
    Image?: string;
    FType!: string;
    SellRent!: number| null;
    BuiltArea!: number;
    CarpetArea?: number;
    Security?: number;
    Maintenance?: number;
    Gated?: number;
    MainEntrance?: string;
    AOP?: string;
    Description?: string;
    PostedOn!: string;
    PostedBy! : number;
    
}
