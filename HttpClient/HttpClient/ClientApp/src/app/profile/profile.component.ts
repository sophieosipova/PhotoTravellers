
import { Component, Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Profile } from '../shared/profile';
import { IPager } from '../shared/pager.model';

@Component({
  selector: 'profile-data',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css'],
  styles: [`
        input.ng-touched.ng-invalid {border:solid red 2px;}
        input.ng-touched.ng-valid {border:solid green 2px;}
    `]
})




export class ProfileComponent {
  error: any;
  serverError: boolean = false;
  profile: Profile = new Profile();
  
  paginationInfo: IPager;
  http: HttpClient;
  profileUrl: string;

  tableMode: boolean = false;
  editMode: boolean = false;
  fullMode: boolean = false;
  userId: string;
  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {

    this.profileUrl = "https://localhost:44361/api/profile";


    this.http = http;
    // let tokenInfo = JSON.parse(localStorage.getItem('TokenInfo'));
    //this.productUrl = 'https://localhost:44350/api/products/all' + `/user/${tokenInfo.userId}`;
   // this.userId = tokenInfo.userId;
  }


  ngOnInit() {
    this.editProfile('8a0a7e1b-9543-4c29-9db2-20ebad6527f7');
  }


  cancel() {

    this.tableMode = true;
    this.editMode = false;
    this.fullMode = false;
  }



  editProfile(profileId : string) {
    this.serverError = false;
    this.error = null;
    let url = this.profileUrl + '/'+ profileId;

    this.http.get<Profile>(url).subscribe(result => {
      this.profile = result     
    }, error => { this.serverError = true, this.error = error, console.error(error); });
  }


}

