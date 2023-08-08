export class MenuModel {
    id: Number;
    title: string;
    parent: number;
    level: string;
    heirarchy: string;
    actualpath: string;
    children: MenuModel[] = [];
    url: string;
}
