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
      this.datas.forEach((element:any) => {
        element.dataUrl = "data:" + element.fileType + ';base64,' + element.image
      });
    });
  }

  getImages(){
    this.api.get('/api/Picture/Get').subscribe((res:any)=>{
      debugger
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
debugger
    this.onSelectFile(files);

    let fileToUpload = <File>files[0];
    const formData = new FormData();
    formData.append('file', fileToUpload, fileToUpload.name);
    formData.append('fileString',  this.dataUrl);
    const _url = this.api.getServerUrl('') + '/api/Picture/UploadImage';
    let body = null;
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

  mimeType:any;
  dataUrl:any;
  typeOneFile:any;
  sizeOneFile:any;
  onSelectFile(eventImage:any) {
      this.dataUrl = '';
      this.typeOneFile = eventImage[0].type;
      this.sizeOneFile = eventImage[0].size;

      var reader: FileReader = new FileReader();
    //   reader.onload =  (readerEvt: any) =>{
    //     var binaryString = readerEvt.target.result;
    //     this.dataUrl = binaryString;
    //     this.mimeType = this.dataUrl.substring(this.dataUrl.lastIndexOf("data") + 5, this.dataUrl.lastIndexOf(";"));
    //     this.dataUrl = this.dataUrl.substring(this.dataUrl.lastIndexOf("base64") + 7);
    // }

    reader.onload = (e) => {
      this.dataUrl=reader.result;
  }

    reader.readAsDataURL(eventImage[0]);
}


}
