export interface IPropertyBase {
    id: number | null;
    sellRent: number | null;
    name: string | null;
    propertyType: string | null;
    furnishingType: string | null;
    bhk: number | null;
    city: string | null;
    readyToMove: boolean | null;
    builtArea: number | null;
    price: number | null;
    photo?: string | null;
    estPossessionOn?: string;
}
