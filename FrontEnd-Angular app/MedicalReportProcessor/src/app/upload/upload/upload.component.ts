import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, NgForm } from '@angular/forms';
import { PageEvent } from '@angular/material/paginator';
import { catchError, of, Subject, switchMap, take, takeUntil } from 'rxjs';
import { UploadService } from './upload.service';

@Component({
  selector: 'app-upload',
  templateUrl: './upload.component.html',
  styleUrls: ['./upload.component.css']
})
export class UploadComponent implements OnInit, OnDestroy {
  @ViewChild('uploadNgForm') uploadNgForm: NgForm;
  lifeEnd$: Subject<any> = new Subject();
  form: FormGroup;
  errorMsg: string = '';
  fileFormats: string = ".txt, .edi, .x12, .edifact, .asc";
  medicalRecords: any[] = [];
  displayedColumns:string[] = [ "name", "fileSize", "uploadedAt", "uploadStatus", "total", "view"];
  reportLength: number = 0;
  pageIndex: number = 0;
  pageSize: number = 10;

  constructor(private fb: FormBuilder,
    private uploadSrv: UploadService){

  }
  ngOnDestroy(): void {
    this.lifeEnd$.next(true);
    this.lifeEnd$.complete();
  }
  ngOnInit(): void {
    this.form = this.fb.group({
      file: ['']
    });

    this.uploadSrv.getReports(this.pageIndex, this.pageSize).pipe(
      takeUntil(this.lifeEnd$)
    ).subscribe((resp) => {
        if(resp && resp.data && resp.total) {
          this.reportLength = resp.total;
          this.medicalRecords = resp.data;
        }
    });
  }

  upload(): void {

    let formData = new FormData();
    for(var key in this.form.value) {
      if(this.form.value[key] !== '') {
        formData.append(key, this.form.value[key]);
      }
    }

    this.uploadSrv.upload(formData)
        .pipe(
          take(1),
          catchError((err) => of("The upload failed because of an internal error. Please try again")),
          switchMap((resp: any) => {
            if(resp.success && resp.parseSuccess)
               return of("The upload has completed successfully")
            return of("The upload failed because of an internal error. Please try again")
          })).subscribe((msg) => {
            alert(msg);
            this.clear();
            location.reload();
          });
  }

  clear(): void {
    this.uploadNgForm.resetForm();
  }

  handlePageEvent(evt: PageEvent) {
    this.pageIndex = evt.pageIndex;
    this.pageSize = evt.pageSize;

    this.uploadSrv.getReports(this.pageIndex, this.pageSize).pipe(
      take(1)
    ).subscribe((resp) => {
        if(resp && resp.data && resp.total) {
          this.reportLength = resp.total;
          this.medicalRecords = resp.data;
        }
    });
  }
}
