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
    this.getDatasApi2();
  }

  getDatas(){
    this.api.get('/api/WeatherForecast/Get').subscribe((res:any)=>{
      this.datas = res
      this.datas.forEach((element:any) => {
        element.dataUrl = "data:" + element.fileType + ';base64,' + element.image
      });
    });
  }

  api2DtaTest:any;
  getDatasApi2(){
    debugger
    this.api.get('/api/WebApi2Config/GetTestData').subscribe((res:any)=>{
    debugger
      this.api2DtaTest = res
    });
  }

  getImages(){
    this.api.get('/api/Picture/Get').subscribe((res:any)=>{
      debugger

      let finalRes : any[] = []

      res.forEach((element:any) => {
        let finalRes1;
        finalRes1 = {...element};
        finalRes1.dataUrl = "data:" + element.fileType + ';base64,' + element.dbImage;
        finalRes.push(finalRes1);
      });

      this.images = finalRes
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
      reader.onloadend = (e:any)=>{
        console.log('reader.onloadend');
        console.log(e);
        let ddd=e.currentTarget.result;
        debugger
        console.log('ddd: '+ ddd);



        let fileToUpload = <File>eventImage[0];
        const formData = new FormData();
        formData.append('file', fileToUpload, fileToUpload.name);
        formData.append('fileString',  ddd);
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
    this.getImages();

            }
          },
          error: (err: HttpErrorResponse) => console.log(err)
        });




      }
      reader.onload = (e:any) => {
        console.log('reader.onload');
        console.log(e);
      this.dataUrl=e.result;
  }

    reader.readAsDataURL(eventImage[0]);
    console.log(this.dataUrl);

}

remove(fileName:any){
  const formData = new FormData();
  formData.append('imageName', fileName);
  const _url = this.api.getServerUrl('') + '/api/Picture/RemoveImage';

  this.http.post(_url, formData).subscribe((res) =>{
    debugger
    if(res == "OK"){
      this.message = 'Remove success.';
      this.getImages();
    }else{
      this.message = 'Remove Failed.';
    }
  });
}


}
