import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { ApiService } from '../services/api.service';
import { HttpClient, HttpEventType, HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-first-page',
  templateUrl: './first-page.component.html',
  styleUrls: ['./first-page.component.scss']
})
export class FirstPageComponent implements OnInit {

  datas:any;
  images:any;

  constructor(private api: ApiService,private http: HttpClient) { }

  ngOnInit(): void {
    this.getDatas();
    this.getImages();
  }

  getDatas(){
    this.api.get('/api/WeatherForecast/Get').subscribe((res:any)=>{
      this.datas = res
    });
  }

  getImages(){
    this.api.get('/api/Picture/Get').subscribe((res:any)=>{
      this.images = res
    });
  }













  @Output() public onUploadFinished = new EventEmitter();
  progress: any;
  message: any;
  uploadFile = (files:any) => {
    if (files.length === 0) {
      return;
    }
    let fileToUpload = <File>files[0];
    const formData = new FormData();
    formData.append('file', fileToUpload, fileToUpload.name);
    const _url = this.api.getServerUrl('') + '/api/Picture/UploadImage';
    this.http.post(_url, formData, {reportProgress: true, observe: 'events'})
      .subscribe({
        next: (event : any) => {
        if (event.type === HttpEventType.UploadProgress)
          this.progress = Math.round(100 * event.loaded / event.total);
        else if (event.type === HttpEventType.Response) {
          this.message = 'Upload success.';
          this.onUploadFinished.emit(event.body);
        }
      },
      error: (err: HttpErrorResponse) => console.log(err)
    });

  }






}
