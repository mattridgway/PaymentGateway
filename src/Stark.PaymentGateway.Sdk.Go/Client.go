package PaymentGatewaySDK

import (
	"encoding/json"
	"net/http"
	"time"
	"fmt"
)

type Client struct {
	Token string
	BaseURL string
	HTTPClient *http.Client
}

type PaymentDetail struct {
	Id			string	`json:"id"`
	MerchantId	string 	`json:"merchantId"`
	When		string	`json:"when"`
	Status		string	`json:"status"`
	IsSuccess	bool	`json:"isSuccess"`
	CardNumber	string	`json:"maskedCardNumber"`
	Amount		float32	`json:"amount"`
	Currency	string	`json:"currency"`
}

func NewClient(token string) *Client {
	return &Client{
		Token: token,
		BaseURL: "https://localhost:5001/api/v1.0",
		HTTPClient: &http.Client{
			Timeout: time.Minute,
		},
	}
}

func (c *Client) sendRequest(req *http.Request, v interface{}) (error) {
	req.Header.Set("Accept", "application/json; charset=utf-8")
	req.Header.Set("Authorization", fmt.Sprintf("Bearer %s", c.Token))

	res, err := c.HTTPClient.Do(req)
	if err != nil {
		return err
	}

	defer res.Body.Close()

	if err = json.NewDecoder(res.Body).Decode(v); err != nil {
		return err
	}

	return nil
}