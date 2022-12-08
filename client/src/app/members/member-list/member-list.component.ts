import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { Pagination } from '../../_models/pagination';
import { Member } from '../../_models/member';
import { MembersService } from '../../_services/members.service';

import { AccountService } from './../../_services/account.service';
import { take } from 'rxjs/operators';
import { User } from '../../_models/user';
import { UserParams } from '../../_models/UserParams';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  //  members$:Observable<Member[]>|undefined;
  members: Member[] | undefined;
  pagination: Pagination | undefined;
  userParams: UserParams | undefined;
  user: User | undefined;
  genderList = [{ value: 'male', display: 'Males' }, { value: 'female', display: 'Females' }];

  constructor(private memberService: MembersService) {
    this.userParams = this.memberService.getUserParams();
  }


  ngOnInit(): void {
    //this.members$ = this.membersService.getMembers();
    this.loadMembers();
  }

  loadMembers() {
    // this.memberService.getMembers().subscribe( members =>this.members = members )
    if (this.userParams) {
      this.memberService.setUserParams(this.userParams);
      this.memberService.getMembers(this.userParams).subscribe({
        next: response => {
          if (response.result && response.pagination) {
            this.members = response.result;
            this.pagination = response.pagination;
          }
        }
      })
    }
  }



  resetUserParams() {
    if (this.user) {
      this.userParams = new UserParams(this.user);
      return this.userParams;
    }
    return;
  }

  resetFilters() {
    this.userParams = this.memberService.resetUserParams();
    this.loadMembers();
  }

  pageChanged(event: any) {
    if (this.userParams && this.userParams?.pageNumber !== event.page) {
      this.userParams.pageNumber = event.page;
      this.memberService.setUserParams(this.userParams);
      this.loadMembers();
    }
  }
}
