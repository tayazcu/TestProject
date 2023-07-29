import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, throwError } from 'rxjs';

import { UrlConfigService } from './url-config.service';
import { serverResponse } from './serverResponse';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  serverName: any;

  constructor(private _http: HttpClient,
              private _urlConfig:UrlConfigService)
              {}

  public getServerUrl(serverName: string): string {
    switch (serverName) {
      case '':
        return this._urlConfig.webApiUrl;
      default:
      return this._urlConfig.webApiUrl;
    }
  }

  get<T>(getURL: string,ServerName: string = '' , params? : HttpParams) {
    const _url = this.getServerUrl(ServerName) + getURL;
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    });
    return this._http.get<serverResponse<T>>(_url, {
      headers : headers,
      params : params
    })
    .pipe(
      catchError(error => {
        if(!error.error){
          error.error = {errorHandled : false}
        }
      return throwError(error || 'Server error');
    })
    );
  }

  // post<T>(data: any, getURL: string,ServerName: string, params : HttpParams = new HttpParams()) {
  //   const _url = this.getServerUrl(ServerName) + getURL;
  //   const body = JSON.stringify(data);
  //   const headers = new HttpHeaders({
  //     'Content-Type': 'application/json',
  //     Authorization: this.oauth.authorizationHeader()
  //   });
  //   return this._http.post<serverResponse<T>>(_url , body,
  //      {headers : headers,
  //       params:params})
  //   .pipe(
  //     catchError(error => {
  //       if(!error.error){
  //         error.error = {errorHandled : false}
  //       }
  //     error.error.errorHandled = this.errorHandler.handleError(error);
  //     return throwError(error || 'Server error');
  //   })
  //   );
  // }

  // postForm<T>( getURL: string,ServerName: string, formData : FormData = new FormData ,params : HttpParams = new HttpParams()) {
  //   const _url = this.getServerUrl(ServerName) + getURL;
  //   const headers = new HttpHeaders({
  //     Authorization: this.oauth.authorizationHeader()
  //   });
  //   return this._http.post<serverResponse<T>>(_url , formData,
  //      {headers : headers,
  //       params:params})
  //   .pipe(
  //     catchError(error => {
  //       if(!error.error){
  //         error.error = {errorHandled : false}
  //       }
  //     error.error.errorHandled = this.errorHandler.handleError(error);
  //     return throwError(error || 'Server error');
  //   })
  //   );
  // }


  // public httpGetAsBlob(
  //   url: string,
  // ): Observable<string | ArrayBuffer> {
  //   return new Observable((subscriber) => {
  //     const headers = new HttpHeaders({
  //       'Content-Type': 'application/json',
  //       Authorization: this.oauth.authorizationHeader()
  //     }
  //     );
  //     this._http
  //       .get(url, { headers: headers, responseType: "blob"})
  //       .subscribe((response: any) => {
  //         const reader = new FileReader();
  //         reader.readAsDataURL(response);
  //         reader.onloadend = function(){
  //           subscriber.next(reader.result?.toString());
  //           subscriber.complete();
  //         };
  //       });
  //   });
  // }


  // public httpGetAsBlobWithReportProgress(
  //   getURL: string,
  //   serverName: string,
  //   params : HttpParams = new HttpParams()
  // ): Observable<any> {
  //   const headers = new HttpHeaders({
  //     Authorization: this.oauth.authorizationHeader()
  //   }
  //   );
  //   const _url = this.getServerUrl(serverName) + getURL;
  //   return this._http.get(_url, {
  //     headers: headers,
  //     responseType: "blob",
  //     reportProgress: true,
  //     observe: "events",
  //     params:params
  //   });
  // }







}

