import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { map, Observable } from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class RecordService {
  apiBase: string = "https://localhost:7243/api/medicalreports";
  constructor(private httpClient: HttpClient) {

  }

  getReports(reportId: number, searchText: string, pageIndex: number, pageSize: number): Observable<any>  {
    const txt = encodeURI(searchText);
    return this.httpClient.get(
      `${this.apiBase}/getMedicalReportDetails?id=${reportId}&searchText=${txt}&pageIndex=${pageIndex}&pageSize=${pageSize}`)
      .pipe(
        map((resp: any) => {
          if(resp.data && resp.data.length > 0) {
            for(let i = 0; i < resp.data.length; i++) {
              if(resp.data[i].reportDetailsErrors) {
                var err = resp.data[i].reportDetailsErrors.map((obj: any) => obj['errorCode']).join(",");
                resp.data[i].error = err;
              }
            }
          }
          return resp;
        })
      );
  }
}
