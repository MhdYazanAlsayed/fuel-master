export interface GenericResultDto<T> {
  success: boolean;
  message?: string;
  data: T;
}

export interface ResultDto {
  success: boolean;
  message?: string;
}
