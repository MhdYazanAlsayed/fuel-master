export default interface IHttpService {
  postData<T>(url: string, data?: T): Promise<Response | null>;
  patchData<T>(url: string, data?: T): Promise<Response | null>;
  putData<T>(url: string, data: T): Promise<Response | null>;
  postDataWithFile(url: string, data: FormData): Promise<Response | null>;
  putDataWithFile(url: string, data: FormData): Promise<Response | null>;
  getData(url: string): Promise<Response | null>;
  deleteData<T>(url: string, data?: T): Promise<Response | null>;
}
