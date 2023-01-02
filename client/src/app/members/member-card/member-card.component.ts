import { Component, Input, OnInit, ViewEncapsulation } from '@angular/core';
import { Member } from './../../_models/member';
import {  MembersService } from './../../_services/members.service'; 
import { ToastrService } from 'ngx-toastr';
import { PresenceService } from './../../_services/presence.service';


@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css'], 
  encapsulation: ViewEncapsulation.ShadowDom
})

export class MemberCardComponent implements OnInit {
@Input() member:Member|undefined;

constructor(private membersService: MembersService, 
  private toastr: ToastrService, public presenceService: PresenceService) {
  
 }

ngOnInit(): void {
  }

addLike(member:Member)
{
  this.membersService.addLike(member.userName).subscribe({
    next: ()=>{
      this.toastr.success("you have like " + member.knownAs)
    }}
  )
}

}
