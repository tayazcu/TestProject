import { Component, OnInit } from '@angular/core';
import { ApiService } from '../services/api.service';

@Component({
  selector: 'app-first-page',
  templateUrl: './first-page.component.html',
  styleUrls: ['./first-page.component.scss']
})
export class FirstPageComponent implements OnInit {

  constructor(private api: ApiService) { }

  ngOnInit(): void {
    this.get();
  }

  list:any;
  get(){
    this.api.get('/api/WeatherForecast/Get').subscribe((res:any)=>{
      this.list = res
      console.log(res);

    });
  }

}
