import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class UploadService {
  apiBase: string = "https://localhost:7243/api/medicalreports";
  constructor(private httpClient: HttpClient) {

  }

  getReports(pageIndex: number, pageSize: number): Observable<any>  {
    return this.httpClient.get(
      `${this.apiBase}/getMedicalReports?pageIndex=${pageIndex}&pageSize=${pageSize}`);
  }

  upload(formData: any): Observable<any> {
    const headers = new HttpHeaders().append("Content-Disposition", "multipart/form-data");
    return this.httpClient.post(
      `${this.apiBase}/postMedicalReport`,
      formData,
      {
        headers: headers
      });
  }
}
