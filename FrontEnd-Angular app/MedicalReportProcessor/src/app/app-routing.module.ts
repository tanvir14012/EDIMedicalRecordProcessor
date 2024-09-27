import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RecordsComponent } from './upload/records/records.component';

const routes: Routes = [
  {
    path: "",
    children: [
      { path: "upload", loadChildren: () => import("src/app/upload/upload.module").then(m => m.UploadModule)},
      { path: "record/:id", component: RecordsComponent}
    ]
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
