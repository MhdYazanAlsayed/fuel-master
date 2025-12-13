import type ITenantService from "../../core/interfaces/apis/tenant/ITenantService";
import type CreateTenantDto from "../../core/interfaces/apis/tenant/dtos/CreateTenantDto";
import type IHttpService from "../../core/interfaces/http/IHttpService";
import type ILoggerService from "../../core/interfaces/loggers/ILoggerService";
import { Services } from "../../core/utils/ServiceCollection";
import DependeciesInjector from "../../core/utils/DependeciesInjector";
import { ResultDto } from "../../core/dtos/ResultDto";

export default class TenantService implements ITenantService {
  private readonly _httpService: IHttpService;
  private readonly _logger: ILoggerService;

  constructor() {
    this._httpService = DependeciesInjector.services.getService<IHttpService>(
      Services.HttpService
    );
    this._logger = DependeciesInjector.services.getService<ILoggerService>(
      Services.LoggerService
    );
  }

  async createTenant(createTenantDto: CreateTenantDto): Promise<ResultDto> {
    try {
      const response = await this._httpService.postData<CreateTenantDto>(
        "/api/tenant",
        createTenantDto
      );

      if (!response) {
        this._logger.logError("Failed to create tenant: No response from server");
        return { success: false, message: "No response from server." };
      }

      if (!response.ok) {
        const errorData = await response.json().catch(() => ({}));
        const errorMessage =
          errorData.message ||
          (errorData.errors
            ? Object.values(errorData.errors).flat().join(" ")
            : `Tenant creation failed with status ${response.status}`);
        this._logger.logError(`Tenant creation failed: ${errorMessage}`);
        return { success: false, message: errorMessage };
      }

      this._logger.logInformation("Tenant created successfully");
      return { success: true };
    } catch (error) {
      const errorMessage =
        error instanceof Error
          ? error.message
          : "An unexpected error occurred during tenant creation.";
      this._logger.logError(`Tenant creation error: ${errorMessage}`);
      return { success: false, message: errorMessage };
    }
  }
}

