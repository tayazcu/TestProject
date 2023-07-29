import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UrlConfigService {
  private config: any = null;
  constructor(
    private http: HttpClient
  ) {}

  get webApiUrl() {
    return this.getProperty("webApi_url");
  }

  load(): Promise<any> {
    return new Promise((resolve,reject)=>{
      this.http
      .get("assets/json/HostConfig.json")
      .toPromise().then((c)=>{
        this.config = c;
        resolve(this.config)
      }).catch(()=>{
        reject()
      })
    });
  }
  private getProperty(property: string): any {
    //noinspection TsLint
    if (!this.config) {
      throw new Error(
        "Attempted to access configuration property before configuration data was loaded, please implemented."
      );
    }

    if (!this.config[property]) {
      throw new Error(`Required property ${property} was not defined within the configuration object. Please double check the
        assets/config.json file`);
    }

    return this.config[property];
  }
}
