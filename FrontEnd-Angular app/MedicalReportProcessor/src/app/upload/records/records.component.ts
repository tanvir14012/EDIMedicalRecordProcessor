import { AfterViewInit, Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { PageEvent } from '@angular/material/paginator';
import { ActivatedRoute } from '@angular/router';
import { debounceTime, Subject, take, takeUntil } from 'rxjs';
import { UploadService } from '../upload/upload.service';
import { RecordService } from './records.service';

@Component({
  selector: 'app-records',
  templateUrl: './records.component.html',
  styleUrls: ['./records.component.css']
})
export class RecordsComponent implements OnInit, OnDestroy, AfterViewInit {
  lifeEnd$: Subject<any> = new Subject();
  searchForm: FormGroup;
  errorMsg: string = '';
  records: any[] = [];
  displayedColumns:string[] = [ "contractId", "recordId", "status", "error"];
  reportLength: number = 0;
  pageIndex: number = 0;
  pageSize: number = 10;
  id: number = 0;

  constructor(private recordSrv: RecordService,
    private route: ActivatedRoute,
    private fb: FormBuilder) {

  }
  ngAfterViewInit(): void {
    this.route.params.pipe(
      takeUntil(this.lifeEnd$)
    ).subscribe(params => {
      let id = params['id'];
      if(id) {
        this.id = id;

        this.fetchData();
      }
    });

    this.searchForm.get('searchField')?.valueChanges.pipe(
      debounceTime(300),
      takeUntil(this.lifeEnd$)
    ).subscribe(() => {
      this.fetchData();
    });

  }

  ngOnInit(): void {
    this.searchForm = this.fb.group({
      searchField: ['']
    })
  }

  ngOnDestroy(): void {
    this.lifeEnd$.next(true);
    this.lifeEnd$.complete();
  }

  handlePageEvent(evt: PageEvent) {
    this.pageIndex = evt.pageIndex;
    this.pageSize = evt.pageSize;

    this.fetchData();
  }

  private fetchData(): void {
    this.recordSrv.getReports(this.id, this.searchForm.get('searchField')?.value, this.pageIndex, this.pageSize).pipe(
      take(1)
    ).subscribe((resp) => {
      console.log(resp.data);
        if(resp && resp.data && resp.total) {
          this.reportLength = resp.total;
          this.records = resp.data;
        }
    });
  }

}
