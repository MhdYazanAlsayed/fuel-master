import type { ResultDto } from "../../dtos/ResultDto";
import type CreateTenantDto from "./dtos/CreateTenantDto";

export default interface ITenantService {
  createTenant(createTenantDto: CreateTenantDto): Promise<ResultDto>;
}

