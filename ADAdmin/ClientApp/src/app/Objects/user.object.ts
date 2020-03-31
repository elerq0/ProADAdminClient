export class UserObject {
  name: string;

  constructor(object: any) {
    this.name = object.Name;
  }
}
