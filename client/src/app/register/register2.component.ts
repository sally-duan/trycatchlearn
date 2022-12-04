import { Component, OnInit} from '@angular/core';

import { Input, Output, EventEmitter } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-register',
  templateUrl: './register2.component.html',
  styleUrls: ['./register2.component.css']
})
export class RegisterComponent implements OnInit {

  @Input() usersFromParentComponent:any;
  @Output() cancelRegistraton = new EventEmitter();
  model:any ={}
  

  constructor(private http:HttpClient, private accountService: AccountService, private toastr:ToastrService) { }

  ngOnInit(): void {
  }

  register(){
  this.accountService.register(this.model).subscribe(
    response=>{
      console.log(response);
      this.cancel();
    },
    error=>{
      console.log(error);
      this.toastr.error(error.error);
    }
  )  
  }

  cancel(){
    this.cancelRegistraton.emit(false);
  }

}
