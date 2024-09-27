import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UploadComponent } from './upload/upload.component';
import { UploadRoutingModule } from './upload/upload.routing-module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { NgxMatFileInputModule } from '@angular-material-components/file-input';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { BrowserModule } from '@angular/platform-browser';
import { MatIconModule } from '@angular/material/icon';
import { HttpClientModule } from '@angular/common/http';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { NgxFilesizeModule } from 'ngx-filesize';
import { RecordsComponent } from './records/records.component';

@NgModule({
  declarations: [
    UploadComponent,
    RecordsComponent
  ],
  imports: [
    CommonModule,
    UploadRoutingModule,
    ReactiveFormsModule,
    FormsModule,
    MatFormFieldModule,
    NgxMatFileInputModule,
    MatInputModule,
    MatButtonModule,
    MatCardModule,
    MatIconModule,
    HttpClientModule,
    MatTableModule,
    MatPaginatorModule,
    NgxFilesizeModule,
    MatIconModule
  ]
})
export class UploadModule { }
