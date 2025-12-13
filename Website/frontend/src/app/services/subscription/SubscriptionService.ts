import type ISubscriptionService from "../../core/interfaces/apis/subscription/ISubscriptionService";
import type SubscriptionPlan from "../../core/interfaces/apis/subscription/dtos/SubscriptionPlan";
import type SubscribeDto from "../../core/interfaces/apis/subscription/dtos/SubscribeDto";
import type IHttpService from "../../core/interfaces/http/IHttpService";
import type ILoggerService from "../../core/interfaces/loggers/ILoggerService";
import { Services } from "../../core/utils/ServiceCollection";
import DependeciesInjector from "../../core/utils/DependeciesInjector";
import { ResultDto } from "../../core/dtos/ResultDto";

export default class SubscriptionService implements ISubscriptionService {
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

  async getPlans(activeOnly: boolean = true): Promise<SubscriptionPlan[]> {
    try {
      const url = `/api/subscription/plans?activeOnly=${activeOnly}`;
      const response = await this._httpService.getData(url);

      if (!response) {
        this._logger.logError("Failed to get plans: No response from server");
        return [];
      }

      if (!response.ok) {
        const errorData = await response.json().catch(() => ({}));
        const errorMessage =
          errorData.message || `Failed to get plans with status ${response.status}`;
        this._logger.logError(`Get plans failed: ${errorMessage}`);
        return [];
      }

      const plans: SubscriptionPlan[] = await response.json();
      this._logger.logInformation(`Retrieved ${plans.length} subscription plans`);
      return plans;
    } catch (error) {
      const errorMessage =
        error instanceof Error
          ? error.message
          : "An unexpected error occurred while fetching plans.";
      this._logger.logError(`Get plans error: ${errorMessage}`);
      return [];
    }
  }

  async subscribe(subscribeDto: SubscribeDto): Promise<ResultDto> {
    try {
      const response = await this._httpService.postData<SubscribeDto>(
        "/api/subscription/subscribe",
        subscribeDto
      );

      if (!response) {
        this._logger.logError("Failed to subscribe: No response from server");
        return { success: false, message: "No response from server." };
      }

      if (!response.ok) {
        const errorData = await response.json().catch(() => ({}));
        const errorMessage =
          errorData.message ||
          (errorData.errors
            ? Object.values(errorData.errors).flat().join(" ")
            : `Subscription failed with status ${response.status}`);
        this._logger.logError(`Subscription failed: ${errorMessage}`);
        return { success: false, message: errorMessage };
      }

      this._logger.logInformation("User subscribed successfully");
      return { success: true };
    } catch (error) {
      const errorMessage =
        error instanceof Error
          ? error.message
          : "An unexpected error occurred during subscription.";
      this._logger.logError(`Subscription error: ${errorMessage}`);
      return { success: false, message: errorMessage };
    }
  }
}

