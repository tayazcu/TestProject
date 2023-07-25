export class serverResponse<T>{
    darkhastTypes: any;
    split(arg0: string) {
      throw new Error('Method not implemented.');
    }
    constructor(){}
    isSuccess! : boolean;
    statusCode! : string;
    message! : string;
    messageWithException! : string;
    errors! : string[];
    data! : T;
    errorHandled!:boolean;
  }
