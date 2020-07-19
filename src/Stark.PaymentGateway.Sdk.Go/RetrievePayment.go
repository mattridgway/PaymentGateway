package PaymentGatewaySDK

import (
	"net/http"
	"fmt"
)

func (c *Client) Retrieve(id string) (*PaymentDetail, error) {
	req, err := http.NewRequest("GET", fmt.Sprintf("%s/payments/%s", c.BaseURL, id), nil)
	if err != nil {
		return nil, err
	}

	res:= PaymentDetail{}
	if err := c.sendRequest(req, &res); err != nil {
		return nil, err
	}

	return &res, nil
}