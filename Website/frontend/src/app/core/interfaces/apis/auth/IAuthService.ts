import { ResultDto } from "../../../dtos/ResultDto";
import LoginDto from "./dtos/LoginDto";
import RegisterDto from "./dtos/RegisterDto";

export default interface IAuthService {
  register(registerDto: RegisterDto): Promise<ResultDto>;
  login(loginDto: LoginDto): Promise<ResultDto>;
}
